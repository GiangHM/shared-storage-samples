<script setup lang="ts">
import DocumentTypeTable from '../components/DocumentTable.vue'
import SasToken from '@/components/SasToken.vue';
import { ref } from 'vue';

const value = ref(null);
</script>

<template>
  <div class="greetings">
    <h1 class="green">Document management</h1>
    <h3 class="green">Documents</h3>
    <DocumentTypeTable />
    <h3 class="green new-topic">Upload new doc </h3>
    <SasToken/>
  </div>
</template>

<style scoped>
h3 {
  font-size: 1.2rem;
}

</style>
