<template>
  <main-outline>
    <mu-flex direction="column">
      <h1>投票导引</h1>
      <div class="introduction">
        <div>本次投票为提名环节。</div>
        <div>规则： 提名次数不限。提名需要同时提供提名位置和提名台词。</div>
        <div>阵营九宫格的具体涵义和样例台词请参考萌娘百科。台词来源可以是小本本/本人实际台词，也可以是设计台词。</div>
      </div>
      <div class="enter align-self-center">
        <router-link tag="div" to="/vote">
          <mu-button color="primary">进入第三届投票</mu-button>
        </router-link>
      </div>
    </mu-flex>
  </main-outline>
</template>

<script lang="ts">
import MainOutline from '../components/MainOutline.vue'
export default {
  name: 'Intro',
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
