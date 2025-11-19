<script setup lang="ts">
import { RouterView } from 'vue-router'
import './commons/axioInterceptors'
import { VTBusyProgressView, VTDialogView, VTToastView } from '@ocph23/vtocph23';
</script>

<template>
  <VTBusyProgressView />
  <VTDialogView />
  <VTToastView />
  <RouterView />
</template>

<style lang="scss"></style>
