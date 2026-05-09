import azure.functions as func
import logging
import json
from apiconnectorhelper import ApiConnectorHelper
from document_reqmodel import Document_RequestModel
import os
from blobuploader import BlobUploader

app = func.FunctionApp(http_auth_level=func.AuthLevel.ANONYMOUS)

@app.route(route="doc_receiver")
@app.service_bus_topic_output(connection="ServiceBusConnection", topic_name="document-queue")
def doc_receiver(req: func.HttpRequest, outputSbMsg: func.Out[str]) -> func.HttpResponse:

    logging.info('Document receiver function processing request.')
    try:
        # Parse request into Document_RequestModel
        try:
            req_body = req.get_json()
            document_request = Document_RequestModel.from_dict(req_body)
        except ValueError:
            return func.HttpResponse("Invalid request body format.", status_code=400)
        
        # Validate document properties
        if not document_request.document_type:
            return func.HttpResponse("Document type is required.", status_code=400)
        if not document_request.document_name:
            return func.HttpResponse("Document name is required.", status_code=400)
        if not document_request.document_content:
            return func.HttpResponse("Document content is required.", status_code=400)

        # Call API to generate SAS token
        container_name = os.environ.get("DOCUMENTS_CONTAINER_NAME", "documents")
        api_helper = ApiConnectorHelper()
        sas_token, error = api_helper.get_sas_token(container_name, document_request.document_name)
        if error:
            return func.HttpResponse(error, status_code=500)
        
        blob_uploader = BlobUploader()
        # Upload document content to blob storage
        blob_url = blob_uploader.upload_blob(
            container_name=container_name,
            blob_name=document_request.document_name,
            content=document_request.document_content,
            sas_token=sas_token
        )

        if not blob_url:
            return func.HttpResponse("Failed to upload document to blob storage.", status_code=500)
        # Construct message for Service Bus
        message = {
            "documentType": document_request.document_type,
            "documentName": document_request.document_name,
            "blobUrl": blob_url,
            "additionalInfo": document_request.additional_info
        }
        
        # Send message to Service Bus
        outputSbMsg.set(json.dumps(message))
        
        return func.HttpResponse(
            json.dumps({"status": "Success", "blobUrl": blob_url}),
            mimetype="application/json",
            status_code=200
        )
    
    except Exception as ex:
        logging.error(f"Error processing document: {str(ex)}")
        return func.HttpResponse("An error occurred while processing the document.", status_code=500)
    
