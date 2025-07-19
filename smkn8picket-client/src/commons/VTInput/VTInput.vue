<template>
  <div>
    <label v-if="label" :class="[labelClasses]"
      >{{ label }} <span v-if="props.validation" class="text-orange-500">*</span></label
    >
    <div class="flex relative">
      <div
        v-if="$slots.prefix"
        class="w-10 flex absolute inset-y-0 left-0 items-center pl-3 pointer-events-none overflow-hidden"
      >
        <slot name="prefix" />
      </div>
      <input
        v-bind="$attrs"
        v-model="model"
        :disabled="disabled"
        :type="type"
        :required="required"
        :class="[inputClasses, $slots.prefix ? 'pl-10' : '']"
      />
      <div v-if="$slots.suffix" class="absolute right-2.5 bottom-2.5">
        <slot name="suffix" />
      </div>
    </div>
    <p v-if="props.validation">
      <VTInputErrorMessage :messages="props.validation" />
    </p>
  </div>
</template>

<script lang="ts" setup>
import { computed, watch } from 'vue'
import { useVModel } from '@vueuse/core'
import { twMerge } from 'tailwind-merge'
import { defineProps, withDefaults } from 'vue'
import type { InputSize, InputType } from './types'

interface InputProps {
  disabled?: boolean
  label?: string
  modelValue?: unknown
  required?: boolean
  size?: InputSize
  type?: InputType
  validation?: unknown
}

const props = withDefaults(defineProps<InputProps>(), {
  disabled: false,
  label: '',
  required: false,
  size: 'md',
  type: 'text'
})

const dateHandler = (value: unknown) => {
  // if (props.type == 'date' || props.type == 'datetime-local') {
  //   if (tanggal && tanggal.date && tanggal.timezone) {
  //     temp = VTHelper.TanggalToDate(tanggal, 'Ymd')
  //     emit('update:modelValue', temp)
  //   } else {
  //     temp = tanggal
  //   }
  // }
  emit('update:modelValue', value)
}

const model = useVModel(props, 'modelValue')
const emit = defineEmits(['update:modelValue'])
dateHandler(props.modelValue)
watch(model, (newValue) => {
  dateHandler(newValue)
})

// LABEL
const baseLabelClasses = 'block mb-2 text-sm font-medium'

// INPUT
const defaultInputClasses =
  'bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500'
const disabledInputClasses = 'cursor-not-allowed bg-gray-100'

const inputSizeClasses: Record<InputSize, string> = {
  lg: 'p-4',
  md: 'p-2.5 text-sm',
  sm: 'p-2 text-sm'
}

const successInputClasses =
  'bg-green-50 border-green-500 dark:border-green-500 text-green-900 dark:text-green-400 placeholder-green-700 dark:placeholder-green-500 focus:ring-green-500 focus:border-green-500'
const errorInputClasses =
  'bg-red-50 border-red-500 text-red-900 placeholder-red-700 focus:ring-red-500 focus:border-red-500 dark:text-red-500 dark:placeholder-red-500 dark:border-red-500'

const inputClasses = computed(() => {
  let classByStatus = defaultInputClasses
  if (props.validation) {
    const vsx = null
    classByStatus = vsx ? errorInputClasses : successInputClasses
  }

  return twMerge(
    defaultInputClasses,
    classByStatus,
    inputSizeClasses[props.size],
    props.disabled ? disabledInputClasses : ''
  )
})

const labelClasses = computed(() => {
  let classByStatus = 'text-gray-900 dark:text-white'
  if (props.validation) {
    const vsx = null
    classByStatus = vsx ? 'text-red-700 dark:text-red-500' : 'text-green-700 dark:text-green-500'
  }

  return twMerge(baseLabelClasses, classByStatus)
})
</script>
