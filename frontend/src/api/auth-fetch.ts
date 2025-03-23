// auth-fetch.ts
import { type Ref } from 'vue';

export async function createAuthFetch(
  getAccessTokenSilently: () => Promise<string>,
  isAuthenticated: Ref<boolean>
) {
  return async (url: RequestInfo, init?: RequestInit): Promise<Response> => {
    // Only add auth headers if authenticated
    if (isAuthenticated.value) {
      try {
        const token = await getAccessTokenSilently();
        console.log('token', token);
        
        // Add token to headers
        init = init || {};
        init.headers = {
          ...init?.headers,
          Authorization: `Bearer ${token}`
        };
      } catch (error) {
        console.error('Error getting access token:', error);
      }
    }
    
    return fetch(url, init);
  };
}