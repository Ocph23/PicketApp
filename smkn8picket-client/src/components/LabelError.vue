<script setup lang="ts">
import { ref, defineProps } from 'vue';
import type { ErrorDetail } from '@/commons';

const props = defineProps({ errors: Array<ErrorDetail>, propName: String });
const message = ref("");
if (props.errors && props.propName) {
  const result = props.errors.find(x => x.code.toLocaleLowerCase() === props.propName?.toLocaleLowerCase());
  message.value = result ? result.description : '';
}
</script>

<template>
  <label class="text-red-600 text-xs" v-if="message">{{ message }}</label>
</template>
