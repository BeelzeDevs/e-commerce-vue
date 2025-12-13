/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{vue,js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      colors:{
        title:"#FFFFFF",
        subtitle:"#FFFFFF",
        paragraph:"#FFFFFF",
        background:"rgb(150, 159, 176,0.7);",
        primary: "#3674e7",
        secondary:"#3674e7",
        loader: "#3674e7",
        bgloader: "#83a8ce",
        button:"rgba(64, 58, 180, 1)",
        buttonhover:"rgba(63, 53, 130, 0.9)",
        admin: "#0E1624",
        adminhover:"#1A222F",
        bgContent : "#101828",
        adminborders: "#262E3A"
      },
      backgroundImage:{
        "gradient-cover": "linear-gradient(0deg,rgba(0, 0, 0, 1) -55%, rgba(64, 58, 180, 1) 93%, rgba(64, 58, 180, 0.98) 100%);",
        "admin": "#0E1624"
      }
    },
  },
  plugins: [
    
  ],
}
