// Re-export getAuthenticatedClient from the auth store
import { useAuthStore } from '@/stores/auth'

export async function getAuthenticatedClient() {
  const authStore = useAuthStore()
  return authStore.getAuthenticatedClient()
}
