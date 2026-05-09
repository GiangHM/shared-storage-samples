import logging
import requests
import os

class ApiConnectorHelper:
    @staticmethod
    def get_sas_token(container_name, document_name):
        try:
            api_base_url = os.environ.get("API_ENDPOINT_URL", "https://your-api-endpoint")
            api_url = f"{api_base_url}/api/getSasToken?containerName={container_name}&documentName={document_name}"
            response = requests.get(api_url)
            if response.status_code != 200:
                return None, "Failed to get SAS token."
            
            sas_token = response.text.strip()
            if not sas_token:
                return None, "Empty SAS token received."
            
            return sas_token, None
        except Exception as ex:
            logging.error(f"Error getting SAS token: {str(ex)}")
            return None, f"Error getting SAS token: {str(ex)}"