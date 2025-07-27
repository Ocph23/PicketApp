<script setup lang="ts">
import { RouterView } from 'vue-router'
import './commons/axioInterceptors'
import { ToasProvider, DialogProvider } from '@/commons';

</script>

<template>
  <DialogProvider />
  <ToasProvider />
  <RouterView />
</template>

<style lang="scss"></style>
