<template>
  <div class="home">
    <mu-container>
      <mu-flex direction="column">
        <img class="img-fluid align-self-center" alt="BGMNA logo" src="../assets/logo.jpg" />
        <h1>BGMNA 第三届群员九宫格活动</h1>
        <div class="introduction">
          <div>群成员阵营印象划分——或者说九宫格——是群里的传统活动。主要目的是增进友谊和加深群成员之间的刻板印象。</div>
          <div>
            <a href="https://bgmna.org/2019/08/13/alignment-round-one.html">第一届结果</a>
          </div>
          <div>
            <a href="https://bgmna.org/2019/08/15/alignment-round-two.html">第二届结果</a>
          </div>
        </div>
        <div class="enter align-self-center">
          <router-link tag="div" to="/intro">
            <mu-button large color="primary">进入第三届投票</mu-button>
          </router-link>
        </div>
      </mu-flex>
    </mu-container>
  </div>
</template>

<script>
export default {
  name: 'Home',
  components: {}
}
</script>

<style lang="scss">
.enter {
  margin-top: 10px;
}
.h1 {
  text-align: center;
}
</style>
