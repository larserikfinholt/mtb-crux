// auth-fetch.ts
import { useAuth0 } from '@auth0/auth0-vue';

export async function createAuthFetch() {
  const { getAccessTokenSilently, isAuthenticated } = useAuth0();
  
  return async (url: RequestInfo, init?: RequestInit): Promise<Response> => {
    // Only add auth headers if authenticated
    if (isAuthenticated.value) {
      const token = await getAccessTokenSilently();
      console.log('token', token)
      
      // Add token to headers
      init = init || {};
      init.headers = {
        ...init?.headers,
        Authorization: `Bearer ${token}`
      };
    }
    
    return fetch(url, init);
  };
}