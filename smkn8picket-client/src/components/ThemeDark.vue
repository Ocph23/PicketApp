<template>
  <div @change="changeColor">
    <DarkMoonIcon></DarkMoonIcon>
    <LightMoonIcon></LightMoonIcon>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { DarkMoonIcon, LightMoonIcon } from "@/components/icons";

const theme = ref("dark");

const changeColor = () => {
  if (
    theme.value === "dark" || (!("color-theme" in localStorage) &&
      window.matchMedia("(prefers-color-scheme: dark)").matches)
  ) {
    document.documentElement.classList.add("dark");
  } else {
    document.documentElement.classList.remove("dark");
  }
};
</script>
