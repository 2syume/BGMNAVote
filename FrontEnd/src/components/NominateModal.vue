<template>
  <div class="modal-container">
    <div class="modal-content" v-click-outside="close">
      <div class="modal-header">
        <img
          class="img-back"
          v-if="this.iVoteInfo.userId===-1"
          src="../assets/back_arrow.svg"
          alt="关闭"
          @click="close()"
        />
        <img class="img-back" v-else src="../assets/back_arrow.svg" alt="返回" @click="setUserId(-1)" />

        <p class="modal-header-alignment">{{iVoteInfo.alignment}}</p>
        <div class="search-container">
          <img class="img-search" src="../assets/search.svg" alt="搜索" />
          <input
            class="search search-id"
            type="text"
            v-model.trim="nameFilter"
            v-if="this.iVoteInfo.userId===-1"
            placeholder="输入或搜索群友"
          />
          <input
            class="search search-id"
            type="text"
            v-model.trim="sayingFilter"
            v-else
            :placeholder="'输入或搜索'+ getUsername(iVoteInfo.userId) + '的台词'"
          />
          <img class="img-cross" src="../assets/cross.svg" alt="删除" @click="clearFilter()" />
        </div>
      </div>
      <div class="modal-body">
        <transition name="slide-fade" mode="out-in">
          <simplebar
            class="nameList"
            data-simplebar-auto-hide="false"
            v-if="this.iVoteInfo.userId===-1"
            key="listID"
          >
            <transition-group name="list" tag="div">
              <div
                class="id-container"
                v-for="userId in userIds"
                :key="userId"
                @click="setUserId(userId);setSaying('')"
              >
                <img class="id-avatar" :src="`${publicPath}${userId}.jpg`" alt="logo" />
                <span class="id-text">{{ getUsername(userId) }}</span>
              </div>
            </transition-group>
          </simplebar>
          <simplebar class="sayingList" data-simplebar-auto-hide="false" v-else key="listLine">
            <transition-group name="list" tag="div">
              <div
                class="line-container"
                v-for="saying in sayings"
                :key="saying"
                @click="setSaying(saying);submit()"
              >
                <span class="quotation-mark">“</span>
                <span>{{ saying }}</span>
              </div>
            </transition-group>
            <transition name="fade">
              <div class="button" v-if="sayings.length===0" :class="{'button-inactive': sayingFilter === ''}">
                <span class="button-text" @click="setSaying(sayingFilter);submit()">提交自定义台词</span>
              </div>
            </transition>
          </simplebar>
        </transition>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import Database from '../assets/database'
import UserDatabase from '../assets/UserDatabase'
import Vue from 'vue'
/* eslint-disable @typescript-eslint/ban-ts-ignore */
// @ts-ignore: cannot find a type definition file for simplebar-vue
import simplebar from 'simplebar-vue'
/* eslint-enable @typescript-eslint/ban-ts-ignore */
import 'simplebar/dist/simplebar.min.css'

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
  name: 'NominateModal',
  props: {
    voteInfo: { type: Object as () => Vote }
  },
  data () {
    return {
      iVoteInfo: {
        userId: this.voteInfo.userId,
        alignment: this.voteInfo.alignment,
        saying: this.voteInfo.saying
      } as Vote,
      nameFilter: '',
      sayingFilter: '',
      publicPath: process.env.BASE_URL
    }
  },
  computed: {
    userIds (): Array<number> {
      if (this.nameFilter !== undefined && this.nameFilter !== '') {
        return Object.values(UserDatabase)
          .filter(t => t.usernameCombined.toLowerCase().includes(this.nameFilter.toLowerCase()))
          .map(t => t.id)
      }
      return Object.values(UserDatabase).map(t => t.id)
    },
    sayings (): Array<string> {
      const userId = this.iVoteInfo.userId as keyof typeof Database
      if (userId in Database) {
        if (this.sayingFilter !== undefined && this.sayingFilter !== '') {
          return Database[userId]
            .filter(t => t.toLowerCase().includes(this.sayingFilter.toLowerCase()))
            .map(t => t.trim())
        }
        return Database[userId].map(t => t.trim())
      }
      return []
    }
  },
  methods: {
    getUsername (userIdNum: number) {
      if (userIdNum === -1) {
        return ''
      }
      const userId = userIdNum as keyof typeof UserDatabase
      return UserDatabase[userId].usernameCombined
    },
    submit () {
      this.$emit('update:voteInfo', this.iVoteInfo)
      this.close()
    },
    close () {
      this.$emit('close')
    },
    setUserId (userId: number) {
      this.iVoteInfo.userId = userId
    },
    setSaying (saying: string) {
      this.iVoteInfo.saying = saying
    },
    clearFilter () {
      this.nameFilter = ''
      this.sayingFilter = ''
    }
  },
  components: {
    simplebar
  }
})
</script>

<style lang="scss" scoped>
.modal-container {
  z-index: 1000;
  position: fixed;
  left: 0;
  top: 0;
  right: 0;
  bottom: 0;
  width: 100%;
  height: 100%;
  background: rgba(255, 255, 255, 0.5);
  display: flex;
  align-content: center;
  justify-content: center;
  padding: 5px;
}

.modal-content {
  width: 400px;
  height: 500px;
  margin: auto;
  background: #ffffff;
  box-shadow: 0px 2px 20px rgba(0, 0, 0, 0.15);
  border-radius: 10px;
  overflow: hidden;
  display: flex;
  flex-direction: column;

  [data-simplebar] {
    height: 100%;
  }
}

$animation-time: 0.15s;

.modal-header {
  margin: 0 20px;
  position: relative;

  .img-back {
    position: absolute;
    left: 0;
    top: 0;
    opacity: 0.7;
    margin: 0 -1rem;
    padding: 1rem 1rem;
    cursor: pointer;

    transition: $animation-time ease-in;

    &:hover {
      transition: $animation-time ease-in;
      opacity: 1;
    }
  }

  &-alignment {
    text-align: center;
    font-weight: bold;
    color: #ff4081;
  }
}

.modal-body {
  margin: 0 5px;
  flex-grow: 1;
  overflow-X: hidden;
  overflow-Y: auto;
  position: relative;
}

.search-container {
  display: flex;
  align-items: center;
  width: 100%;
  height: 50px;
  border: 1px solid rgba(34, 34, 34, 0.2);
  box-sizing: border-box;
  border-radius: 10px;
  margin-bottom: 20px;
  position: relative;

  .img-search {
    margin: 13px 25px 13px 10px;
    transition: $animation-time ease-in;
    opacity: 0.7;
  }

  .img-cross {
    position: absolute;
    right: 0;
    top: 0;
    bottom: 0;
    margin: 13px 10px;
    opacity: 0;
    transition: $animation-time ease-in;
  }

  .search {
    flex-grow: 1;
    max-width: 280px;
    border: none;
    transition: $animation-time ease-in;
    &:focus {
      outline: none;
    }
  }

  ::placeholder {
    color: rgba($color: #222222, $alpha: 0.6);
  }

  &:focus-within {
    transition: $animation-time ease-in;
    height: 50px;
    outline: none;
    border: 1px solid #ff4081;

    .img-search {
      transition: $animation-time ease-in;
      opacity: 1;
    }

    .img-cross {
      transition: $animation-time ease-in;
      opacity: 0.7;
    }
  }
}

.id-container {
  cursor: pointer;
  width: 100%;
  height: 50px;
  background: #fafafa;
  border: 1px solid rgba(34, 34, 34, 0.3);
  box-sizing: border-box;
  border-radius: 100px;
  display: flex;
  margin-bottom: 10px;
  transition: $animation-time ease-in;
  cursor: pointer;

  .id-avatar {
    width: 38px;
    height: 38px;
    border-radius: 50%;
    margin: 6px 15px 6px 6px;
  }

  .id-text {
    margin: 13px 0;
    transition: $animation-time ease-in;
    font-weight: bold;
  }

  &:hover {
    box-shadow: 0px 2px 10px rgba(0, 0, 0, 0.2);
    transition: $animation-time ease-in;
    background: #ff4081;
    border: 1px solid #ff4081;

    .id-text {
      color: white;
    }
  }
}

.line-container {
  display: flex;
  cursor: pointer;
  width: 100%;
  padding: 10px 0;
  border-bottom: #e0e0e0 1px solid;
  transition: $animation-time ease-in;

  .quotation-mark {
    font-weight: bolder;
    font-size: 1.2rem;
    color: #ff4081;
    margin-right: 10px;
  }

  &:hover {
    transition: $animation-time ease-in;
    color: #ff4081;
  }
}

.button {
  cursor: pointer;
  display: flex;
  background-color: #ff4081;
  border: 1px solid #ff4081;
  border-radius: 10px;
  padding: 10px;
  margin-bottom: 20px;

  .button-text {
    font-weight: bold;
    margin: auto;
    width: 100%;
    text-align: center;
    color: white;
  }

  &-inactive {
    pointer-events: none;
    border: #e4e4e4 1px solid;
    background-color: #e4e4e4;
    .button-text {
      color: #aaaaaa;
    }
  }
}

.fade-enter-active,
.fade-leave-active {
  transition: all 0.3s ease;
}

.fade-enter,
.fade-leave-to {
  opacity: 0;
}

.list-move {
  transition: transform 0.2s;
}

.list-enter-active,
.list-leave-active {
  transition: all 0.2s;
}
.list-enter,
.list-leave-to {
  opacity: 0;
  transform: translateY(-15px);
}

.nameList,
.sayingList {
  &.slide-fade-enter-active,
  &.slide-fade-leave-active {
    transition: opacity 0.3s;
  }
  &.slide-fade-enter,
  &.slide-fade-leave-to {
    transition: 0.3s ease;
    opacity: 0;
  }
}

.nameList {
  &.slide-fade-enter,
  &.slide-fade-leave-to {
    transform: translateX(-20px);
  }
}

.sayingList {
  &.slide-fade-enter,
  &.slide-fade-leave-to {
    transform: translateX(20px);
  }
}
</style>
<style>
.simplebar-content {
  margin: 0 15px;
}

.simplebar-scrollbar {
  left: 2px;
  opacity: 0.5;
}
</style>
