import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import { type Crux, Client } from '@/api/api-client'

export const useCruxStore = defineStore('crux', () => {
  const items = ref<Array<Crux>>([])

    
    const fetchItems = async () => {
        const client = new Client()
        const data = await client.getCrux()
        items.value = data
    }

  return { items, fetchItems }
})
