<template>
   <a-result status="403" title="403" :sub-title="$t('NotPerrmissionPage')">
      <template #extra>
         <router-link :to="{ name: 'Dashboard' }">
            <a-button type="primary">{{ $t('BackHome') }}</a-button>
         </router-link>

      </template>
   </a-result>
</template>
<script setup>
</script>
<style lang="">

</style>