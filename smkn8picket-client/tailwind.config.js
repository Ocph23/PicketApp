/** @type {import('tailwindcss').Config} */
export default {
  purge: [
    './index.html', './src/**/*.{vue,js,ts,jsx,tsx}',
    'node_modules/flowbite-vue/**/*.{js,jsx,ts,tsx,vue}',
    'node_modules/flowbite/**/*.{js,jsx,ts,tsx}'
  ],
  darkMode: false, // or 'media' or 'class'
  content: [

  ],
  theme: {
    extend: {
      backgroundOpacity: {
        '10': '0.1',
        '20': '0.2',
        '95': '0.95',
      },
      fontFamily: {
        poppins: ['Poppins']
      }
    },
  },
  plugins: [
    require('flowbite/plugin')
  ],
}
