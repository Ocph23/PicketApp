<script setup lang="ts">
import { Helper } from '@/commons';

</script>

<template>
  <img :class="$attrs.class" class="mr-2" :src="Helper.infoSekolah.logo" :alt="Helper.infoSekolah.nama_sekolah" />
</template>
