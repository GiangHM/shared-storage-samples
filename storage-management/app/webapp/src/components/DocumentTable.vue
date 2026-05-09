<script setup lang="ts">
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import { ref, onMounted } from 'vue';
import DocumentService  from '@/services/DocumentService';
import Button from 'primevue/button';

let documentService = new DocumentService();

onMounted(() => {
    documentService.getDocumentFromAPI().then(data => documents.value = data);
});

const documents = ref();
async function refresh() {
    var res = await documentService.getDocumentFromAPI()
    documents.value = res;
};
</script>

<template>
    <div class="table-topic">
        <div class="card">
            <DataTable :value="documents" tableStyle="min-width: 40rem">
                <Column field="docTypeCode" header="Code"></Column>
                <Column field="docUrl" header="Document URL"></Column>
            </DataTable>
        </div>
        <div>
            <Button label="Refresh" @click="refresh" severity="secondary"/>
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

h1 {
  font-weight: 500;
  font-size: 2.6rem;
  position: relative;
  top: -10px;
}

h3 {
  font-size: 1.2rem;
}

.table-topic{
    margin-bottom: 2rem;
}

</style>
