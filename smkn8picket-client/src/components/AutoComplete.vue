<template>
  <div class="w-full">
    <div class="grid grid-cols-5 gap-4 items-end">
      <div class="col-span-4">
        <fwb-input :label="props.label" v-model="data.query" @input="onInput" :placeholder="props.placeholder">
          <template #prefix>
            <svg aria-hidden="true" class="w-5 h-5 text-gray-500 dark:text-gray-400" fill="none" stroke="currentColor"
              viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
              <path d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" stroke-linecap="round" stroke-linejoin="round"
                stroke-width="2" />
            </svg>
          </template>
        </fwb-input>
      </div>
      <fwb-button type="button" class="h-auto" @click="search">cari</fwb-button>
    </div>
    <ul v-if="filteredSuggestions.length">
      <li
        class="cursor-pointer hover:bg-gray-700 hover:text-gray-100 dark:hover:text-gray-900 dark:hover:bg-gray-100 block mb-2 text-sm font-medium text-gray-900 dark:text-white"
        v-for="(suggestion, index) in filteredSuggestions" :key="index" @click="selectSuggestion(suggestion)">
        {{ suggestion.name }}
      </li>
    </ul>
  </div>
</template>

<script setup lang="ts">
import { computed, reactive } from 'vue'
import { FwbInput, FwbButton } from 'flowbite-vue'
import AutoCompleteService from '@/services/AutoCompleteService'
import type { AutoCompleteSuggestion } from '@/models'



const props = defineProps(['query', 'service', 'label', 'placeholder'])
const model = defineModel()
type Suggestion = {
  id: number
  name: string
  data: unknown
}



// const emit = defineEmits(['onSelect'])
const data = reactive({
  query: '',
  suggestions: [] as AutoCompleteSuggestion[],
})

data.query = props.query

const filteredSuggestions = computed(() => {
  if (data.query === '') {
    return []
  }
  return data.suggestions.filter((suggestion) =>
    suggestion.name.toLowerCase().includes(data.query.toLowerCase()),
  )
})

function onInput() {
  if (data.query.length == 3) {
    AutoCompleteService.get(props.service, data.query).then((response) => {
      if (response) {
        data.suggestions = response;
      }
    })
  }
}

function search() {
  AutoCompleteService.get(props.service, data.query).then((response) => {
    if (response) {
      data.suggestions = response;
    }
  })
}

function selectSuggestion(suggestion: Suggestion) {
  data.query = suggestion.name;
  model.value = suggestion
  data.suggestions = []
}


</script>

<style scoped>
input {
  width: 100%;
  padding: 8px;
  box-sizing: border-box;
}

ul {
  list-style-type: none;
  padding: 0;
  margin: 0;
  border: 1px solid #ddd;
}

li {
  padding: 4px;
  cursor: pointer;
}
</style>
