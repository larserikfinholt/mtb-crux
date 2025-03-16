<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useCruxStore } from '@/stores/crux';
import { type Crux } from '@/api/api-client';
import { useRoute } from 'vue-router';

// load the crux details from the api, use route params to get the id
const route = useRoute();
const cruxStore = useCruxStore();
const id = parseInt(route.params.id);

const crux = ref<Crux>();

onMounted(async () => {
    if (cruxStore.items.length === 0) {
        await cruxStore.fetchItems();
    }

    crux.value = cruxStore.items.find(c => c.id === id);
});
</script>

<template>
  <div>
    <h1>{{ crux?.name }}</h1>
    <p>{{ crux?.description }}</p>
  </div>
</template>