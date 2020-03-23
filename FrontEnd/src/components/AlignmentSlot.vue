<template>
  <mu-container justify-content="center" align-items="center">
    <div v-if="voteInfo.userId===-1" class="bg not-selected">
      <div class="yotsuba">
        <div class="yotsuba-container">
          <img class="img-yotsuba" src="../assets/vote.jpg" alt="よつばと" />
        </div>
      </div>
      <div class="left-panel">
        <div class="alignment-line">
          <div class="alignment-text major-text">{{voteInfo.alignment}}</div>
          <mu-tooltip class="icon-info" placement="top" :content="alignmentInfo">
            <mu-icon value="info"></mu-icon>
          </mu-tooltip>
        </div>
        <div class="minor-text">点击添加提名</div>
      </div>
    </div>
    <div v-else class="bg">
      <div class="selected-container">
        <div class="selected-top">
          <img class="img-avatar" :src="`${publicPath}${voteInfo.userId}.jpg`" alt="logo" />
          <div class="top-text">
            <span class="top-name">{{ name }}</span>
            <span class="top-alignment">{{ voteInfo.alignment }}</span>
          </div>
        </div>
        <div class="selected-bot">
          <span class="quotation-mark">“</span>
          <div class="bot-line-container">
            <span class="bot-line">{{ voteInfo.saying }}</span>
          </div>
        </div>
      </div>
    </div>
  </mu-container>
</template>

<script lang="ts">
import Vue from 'vue'
import UserDatabase from '../assets/UserDatabase'
import AlignmentDatabase from '../assets/AlignmentDatabase'

interface Vote {
  userId: number;
  saying: string;
  alignment: string;
}

declare let process: {
  env: {
    BASE_URL: string;
  };
}

export default Vue.extend({
  name: 'AlignmentSlot',
  props: {
    voteInfo: { type: Object as () => Vote }
  },
  data () {
    return {
      publicPath: process.env.BASE_URL
    }
  },
  computed: {
    name () {
      if (this.voteInfo.userId === -1) {
        return ''
      }
      const userId = this.voteInfo.userId as keyof typeof UserDatabase
      return UserDatabase[userId].usernameCombined
    },
    alignmentInfo () {
      const alignment = this.voteInfo.alignment as keyof typeof AlignmentDatabase
      return AlignmentDatabase[alignment]
    }
  }
})
</script>

<style lang="scss" scoped>
.bg {
  overflow: hidden;
  margin: {
    top: 10px;
    bottom: 10px;
    left: 10px;
    right: 10px;
  }
  height: 150px;
  background: #ffffff;
  box-shadow: 0px 1px 8px rgba(0, 0, 0, 0.1);
  /* shadow */
  border-radius: 10px;
  padding: {
    left: 18px;
    right: 18px;
  }

  &.not-selected {
    display: flex;
    justify-content: space-between;
    align-items: center;
  }
  cursor: pointer;

  $animation-time: 0.2s;
  transition: $animation-time ease-in;
  border: 2px solid transparent;
  .img-plus {
    transition: $animation-time ease-in;
    opacity: 0.5;
  }

  .yotsuba {
    z-index: 1;
    position: absolute;
    top: 10px;
    right: 20px;
    bottom: 10px;
    left: 20px;
    overflow: hidden;
    border-radius: 10px;

    &-container {
      width: 100%;
      height: 100%;
      overflow: hidden;
      display: flex;
      align-items: center;
      justify-content: center;
      transition: 0.6s cubic-bezier(0.08, 0.74, 0.19, 0.95);
      opacity: 0.9;

      .img-yotsuba {
        margin: auto;
        max-width: 100%;
        max-height: 100%;
      }
    }
  }

  &:hover {
    transition: $animation-time ease-in;
    //border: 2px solid #ff4081;
    box-shadow: 0px 2px 15px rgba(0, 0, 0, 0.15);

    .yotsuba-container {
      transition: 0.6s cubic-bezier(0.08, 0.74, 0.19, 0.95);
      transform: scale(1.25) translateX(-10px);
      opacity: 0.5;
    }

    .major-text {
      transition: 0.35s ease;
      opacity: 0.9;
    }
  }
}

.not-selected .left-panel {
  z-index: 2;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
}
.major-text {
  transition: 0.35s ease;
  width: 100%;
  font-style: normal;
  font-weight: bolder;
  font-size: 1.2rem;
  line-height: 1.8rem;
  color: #ff4081;
  margin-bottom: 20px;
}
.minor-text {
  font-style: normal;
  font-weight: normal;
  font-size: 1rem;
  line-height: 1.5rem;
  color: rgba(60, 60, 60, 0.4);
}

.alignment-line {
  display: flex;
  align-items: center;
  justify-content: center;

  .icon-info {
    margin-left: 5px;
    font-size: 1.2rem;
    line-height: 1.8rem;
    color: #ff4081;
    margin-bottom: 20px;
    transition: 0.15s ease;
    opacity: 0.5;

    &:hover {
      transition: 0.15s ease;
      opacity: 1;
    }

    @media only screen and (max-width: 768px) {
      opacity: 0;
      pointer-events: none;
    }
  }
}

.selected-container {
  z-index: 2;
  height: 100%;
  padding: 18px 0 10px;
  display: flex;
  flex-direction: column;
}

.selected-top {
  display: flex;

  .img-avatar {
    width: 47px;
    height: 47px;
    border-radius: 50%;
    margin-right: 20px;
  }
  .top-text {
    display: flex;
    flex-direction: column;
    justify-content: space-between;
    .top-name {
      font-weight: bold;
      color: #ff4081;
    }
    .top-alignment {
      font-size: 0.85rem;
      opacity: 0.9;
    }
  }
}

.selected-bot {
  display: flex;
  justify-content: space-between;
  padding-bottom: 10px;
  .quotation-mark {
    font-weight: bolder;
    color: rgba(255, 64, 129, 0.3);
    font-size: 50px;
    transform: translateX(-30px);
    font-style: italic;
  }

  .bot-line-container {
    flex-grow: 1;
    display: flex;
  }

  .bot-line {
    margin: auto 0;
    // font-weight: bold;
    font-weight: 600;
    opacity: 0.9;
    font-style: italic;
  }
}
</style>
<style lang="scss">
.mu-tooltip {
  padding: 12px !important;
  border-radius: 10px !important;
  font-family: "Noto Sans SC", Helvetica, Arial, sans-serif !important;
  max-width: 400px !important;
  font-size: 0.8rem !important;
  line-height: 1.3rem !important;
  font-weight: 400 !important;
  color: rgba(34, 34, 34, 0.9) !important;
  background-color: white !important;
  box-shadow: 0 0 10px rgba($color: #222222, $alpha: 0.2);
}

.mu-tooltip-top-enter,
.mu-tooltip-top-leave-active {
  transform: translate3d(0, 10%, 0) !important;
}
</style>
