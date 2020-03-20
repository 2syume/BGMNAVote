<template>
  <main-outline>
    <mu-flex direction="column">
      <h1>投票完成</h1>
      <div class="introduction">
        <div>感谢您的投票！</div>
      </div>
    </mu-flex>
  </main-outline>
</template>

<script lang="ts">
import MainOutline from '../components/MainOutline.vue'
export default {
  name: 'Thanks',
  components: {
    MainOutline
  }
}
</script>

<style lang="scss">
.introduction {
  text-align: left;
}
.enter {
  margin-top: 40px;
}
</style>
