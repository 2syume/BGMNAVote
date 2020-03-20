<template>
  <mu-container class="main d-flex align-items-center">
      <mu-paper :z-depth="2">
        <slot></slot>
      </mu-paper>
  </mu-container>
</template>

<script>
export default {
  name: 'MainOutline'
}
</script>

<style lang="scss">
.mu-paper {
  padding: {
    top: 40px;
    bottom: 40px;
    left: 30px;
    right: 30px;
  }
  min-height: 500px;
  min-width: 100%;
  color: #666666;
}

.main {
  min-height: 90vh;
}
</style>
