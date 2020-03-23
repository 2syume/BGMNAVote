<template>
  <div>
    <mu-container>
      <mu-row justify-content="center" align-items="center">
        <mu-col class="thanks-content" lg="6" span="12">
          <mu-flex direction="column" justify-content="center" align-items="center">
            <div class="major-text text">提名完成</div>
            <div class="minor-text text">感谢您的提名！请等待后续的投票环节。</div>
          </mu-flex>
        </mu-col>
        <mu-col class="thanks-content yotsuba-container" lg="6" span="12">
          <img class="yotsuba-thanks" src="../assets/thanks.png" />
        </mu-col>
      </mu-row>
    </mu-container>
  </div>
</template>

<script lang="ts">
export default {
  name: 'Thanks'
}
</script>

<style lang="scss" scoped>
.main-view {
  position: fixed;
  left: 0;
  right: 0;
  top: 0;
  bottom: 0;
  display: flex;
  height: 100%;
}

.container {
  margin: auto;
}
.text {
  width: 100%;
}
.major-text {
  font-style: normal;
  font-weight: bold;
  font-size: 2rem;
  line-height: 2.5rem;
  color: #ff4081;
  margin-bottom: 20px;

  @media only screen and (max-width: 1000px) {
    font-size: 1.5rem;
    line-height: 2rem;
  }
}
.minor-text {
  font-style: normal;
  font-weight: normal;
  font-size: 1.5rem;
  line-height: 2rem;
  color: rgba(60, 60, 60, 0.6);

  @media only screen and (max-width: 1000px) {
    font-size: 1rem;
    line-height: 1.2rem;
  }
}

.yotsuba-container {
  display: flex;
}

.yotsuba-thanks {
  margin: auto;
  max-width: 100%;
  max-height: 100%;
}

.thanks-content {
  @media only screen and (max-width: 1000px) {
    padding: 30px;
  }
}
</style>
