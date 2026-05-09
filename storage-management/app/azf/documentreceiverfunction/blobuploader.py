from azure.core.exceptions import ServiceRequestError, ClientAuthenticationError
from azure.storage.blob import BlobClient
from azure.core import  ExponentialRetry
import os
import logging

class BlobUploader:
    def upload_document(self, container_name, document_name, document_content, sas_token):
        try:
            # Create a blob client
            retry_policy = ExponentialRetry(max_retries=3)
            blob_client = BlobClient.from_blob_url(
                blob_url=f"https://{os.environ.get('STORAGE_ACCOUNT_NAME')}.blob.core.windows.net/{container_name}/{document_name}?{sas_token}",
                retry_policy=retry_policy
            )
            
            # Upload the document content
            blob_client.upload_blob(document_content, overwrite=True)
            
            # Return the blob URL
            blob_url = f"https://{os.environ.get('STORAGE_ACCOUNT_NAME')}.blob.core.windows.net/{container_name}/{document_name}"
            return blob_url, None
            
        except (ServiceRequestError, ClientAuthenticationError) as e:
            logging.error(f"Azure Storage error: {str(e)}")
            return None, f"Storage service error: {str(e)}"
        except Exception as e:
            logging.error(f"Blob upload error: {str(e)}")
            return None, f"Failed to upload document: {str(e)}"