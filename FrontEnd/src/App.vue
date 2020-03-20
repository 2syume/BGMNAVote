<template>
  <div id="app">
    <div id="nav" style="display:none">
      <router-link to="/">Home</router-link>|
      <router-link to="/about">About</router-link>
    </div>
    <router-view class="main-view" />
  </div>
</template>

<style lang="scss">
@import url("https://fonts.googleapis.com/css?family=Noto+Sans+SC&display=swap");
@import url("https://fonts.googleapis.com/icon?family=Material+Icons");

#app {
  font-family: "Noto Sans SC", Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  color: #2c3e50;
  font-size: 16px;
  font-weight: 400;
}

h1 {
  font-size: 48px;
  font-weight: 400;
  color: #2c3e50;
}

h2 {
  font-size: 34px;
  font-weight: 400;
  color: #2c3e50;
}

#nav {
  padding: 30px;
}

#nav a {
  font-weight: bold;
  color: #2c3e50;
}

#nav a.router-link-exact-active {
  color: #42b983;
}

.img-fluid {
  max-width: 100%;
  height: auto;
}
</style>
