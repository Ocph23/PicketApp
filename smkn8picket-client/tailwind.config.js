/** @type {import('tailwindcss').Config} */
export default {
  purge: ['./src/**/*.{vue,js,ts,jsx,tsx}'],
  content: [],
  darkMode: false, // or 'media' or 'class'
  theme: {
    screens: {
      hp: '344px',
      sm: '640px',
      md: '768px',
      lg: '1024px',
      xl: '1280px',
      '2xl': '1536px',
    },
    extend: {
      fontFamily: {
        poppins: ['Poppins'],
      },
    },
  },
  plugins: [],
}
