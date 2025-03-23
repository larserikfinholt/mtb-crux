<script setup lang="ts">
import CruxList from '@/components/CruxList.vue';

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
