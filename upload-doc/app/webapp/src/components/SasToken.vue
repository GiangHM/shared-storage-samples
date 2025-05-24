<script setup lang="ts">
import Select from 'primevue/select';
import Button from 'primevue/button';
import { ref } from 'vue';
import SasTokenService from '@/services/SasTokenService';
import DocumentTypeService from '@/services/DocumentTypeService';
import FileUpload from 'primevue/fileupload';
import { BlobClient } from '@azure/storage-blob'
import { onMounted } from 'vue'
import DocumentService  from '@/services/DocumentService';
import DocumentCreationRequestModel from '@/models/DocumentCreationRequestModel';

const fileName = ref(null);
const base64String = ref();
var sasToken = ref("");
let sasTokenService = new SasTokenService();
let documentTypeService = new DocumentTypeService();
let documentService = new DocumentService();

const selectedDocumentType = ref();
const documentTypes = ref();

const fileupload = ref();

onMounted(() => {
  documentTypeService.getDocumentTypeFromAPI().then(data => documentTypes.value = data)
})

async function uploadAndSave() {
    if (fileName.value && base64String.value) {

        sasToken.value = await sasTokenService.getBlobSasToken("testcontainer", fileName.value);

        const content:string = base64String.value;
        const blockBlobClient = new BlobClient(sasToken.value)
        const uploadBlobResponse = await blockBlobClient.getBlockBlobClient().upload(content, content.length);

        await documentService.addNewDocument(new DocumentCreationRequestModel(selectedDocumentType.value.docTypeCode, `testcontainer/${fileName.value}`))
    }
    
};

function onFileSelect(event: { files: any[]; }) {
    const file = event.files[0];
    fileName.value = file.name;

    const reader = new FileReader();
    reader.readAsDataURL(file);

    reader.onload = async (e) => {
        base64String.value = reader.result?.toString().split(",")[1];
    };

}
</script>

<template>
    <div class="flex justify-center">
            <div class="card flex justify-center">
                <FileUpload ref="fileupload" mode="basic" @select="onFileSelect" customUpload name="demo[]" accept="image/*" :maxFileSize="1000000" />
            </div>
            <div class="card flex justify-center">
                <Select v-model="selectedDocumentType" :options="documentTypes" optionLabel="docTypeDescription" placeholder="Select a document type"  />
            </div>
            <div class="card flex justify-center">
                <Button label="Upload & Save" @click="uploadAndSave" severity="secondary" />
            </div>
    </div>
</template>
<style scoped>
.card {
    background: var(--card-bg);
    border: var(--card-border);
    padding: 0.75rem;
    border-radius: 10px;
    margin-bottom: 0.75rem;
}
</style>