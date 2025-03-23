import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import { getAuthenticatedClient} from '@/api/auth-helper'
import { type Crux, Client } from '@/api/api-client'

export const useCruxStore = defineStore('crux', () => {
  const items = ref<Array<Crux>>([])

    
    const fetchItems = async () => {

 


        const authenticatedClient = await getAuthenticatedClient()
        //const client = new Client()
        const data = await authenticatedClient.getCrux()
        items.value = data
    }

  return { items, fetchItems }
})
