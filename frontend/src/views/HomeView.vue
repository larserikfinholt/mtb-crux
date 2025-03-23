<script setup lang="ts">
import CruxList from '@/components/CruxList.vue';

import { useAuth0 } from '@auth0/auth0-vue'

const { user } = useAuth0()

// User info from Auth0
console.log(user.value?.name)
console.log(user.value?.email)
console.log(user.value?.picture)
console.log(user.value?.sub) // unique Auth0 ID

</script>

<template>
  <main>
    <h1>Sykkelcrux</h1>
    <p>Velkommen til Sykkelcrux, hvor du kan se alle sykkelcruxene.</p>

    <div>
      <RouterLink to="/map" class="text-blue-500 hover:text-blue-700">Se alle crux på kartet</RouterLink>
    </div>

    <h3>Siste 10 sykkelcruxene</h3>
    <CruxList></CruxList>
  </main>
</template>
