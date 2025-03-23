import { ref } from 'vue'
import { defineStore } from 'pinia'
import { getAuthenticatedClient } from '@/api/auth-helper'
import { type Crux } from '@/api/api-client'

export const useCruxStore = defineStore('crux', () => {
  const items = ref<Array<Crux>>([])
  const isLoading = ref(false)
  const error = ref<string | null>(null)
    
  const fetchItems = async () => {
    try {
      isLoading.value = true
      error.value = null
      
      const authenticatedClient = await getAuthenticatedClient()
      const data = await authenticatedClient.getCrux()
      items.value = data
    } catch (err) {
      console.error('Error fetching crux data:', err)
      error.value = 'Failed to load data. Please try again.'
    } finally {
      isLoading.value = false
    }
  }

  return { 
    items, 
    isLoading, 
    error, 
    fetchItems 
  }
})
