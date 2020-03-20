<template>
  <div>
    <mu-container class="main d-flex align-items-center">
        <mu-paper :z-depth="2">
          <slot></slot>
        </mu-paper>
    </mu-container>
    <div class="footer">Made by&nbsp;<a href="https://github.com/arition" target="_blank">@arition</a>&nbsp;with ❤️️.</div>
  </div>
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

.footer {
  position: fixed;
  bottom: 0;
  width: 100%;
  text-align: right;
  width: 100%;
  padding: 20px;
  background: #eeeeee;
  display: none;
  @media (min-width: 992px) {
    display: block;
  }
}
</style>
