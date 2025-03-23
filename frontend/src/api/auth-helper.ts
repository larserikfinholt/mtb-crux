// Using the client with auth
import { Client } from './api-client';
import { createAuthFetch } from './auth-fetch';

export async function getAuthenticatedClient() {
    console.log("creating auth client")
  const authFetch = await createAuthFetch();
  return new Client("https://localhost:7044", { fetch: authFetch });
}
