class Document_RequestModel:
    def __init__(self, document_type: str, document_name: str, document_content: bytes):
        self.document_type = document_type
        self.document_name = document_name
        self.document_content = document_content
    
    def to_dict(self):
        return {
            "document_type": self.document_type,
            "document_name": self.document_name,
            "document_content": self.document_content
        }