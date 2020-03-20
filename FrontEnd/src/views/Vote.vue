<template>
  <main-outline>
    <mu-flex direction="column">
      <div class="vote-form">
        <h3>投票</h3>
        <mu-form ref="form" :model="currentInfo" v-on:submit.prevent>
          <mu-row gutter align-items="center">
            <mu-col span="3">我提名群员</mu-col>
            <mu-col span="6">
              <mu-form-item prop="name" label="群员">
                <mu-select
                  v-model="currentInfo.name"
                  prop="name"
                  tags
                  full-width
                  v-on:change="cleanSaying()"
                >
                  <mu-option
                    v-for="username in usernames"
                    :key="username"
                    :label="username"
                    :value="username"
                  ></mu-option>
                </mu-select>
              </mu-form-item>
            </mu-col>
          </mu-row>
          <transition name="fade">
            <div v-show="usernameChosen">
              <mu-row gutter align-items="center">
                <mu-col span="3">我觉得{{currentInfo.name}}的阵营是</mu-col>
                <mu-col span="6">
                  <mu-form-item prop="alignment" label="阵营">
                    <mu-select v-model="currentInfo.alignment" prop="alignment" full-width>
                      <mu-option
                        v-for="alignment in alignments"
                        :key="alignment"
                        :label="alignment"
                        :value="alignment"
                      ></mu-option>
                    </mu-select>
                  </mu-form-item>
                </mu-col>
              </mu-row>
              <mu-row gutter align-items="center">
                <mu-col span="9" offset="3">
                  <alignment-info :alignment="currentInfo.alignment" />
                </mu-col>
              </mu-row>
            </div>
          </transition>
          <transition name="fade">
            <mu-row gutter align-items="center" v-show="alignmentChosen">
              <mu-col span="3">我觉得{{currentInfo.name}}的台词是</mu-col>
              <mu-col span="6">
                <mu-form-item prop="saying" label="台词">
                  <mu-select v-model="currentInfo.saying" prop="saying" tags full-width>
                    <mu-option
                      v-for="saying in sayings"
                      :key="saying"
                      :label="saying"
                      :value="saying"
                    ></mu-option>
                  </mu-select>
                </mu-form-item>
              </mu-col>
            </mu-row>
          </transition>
          <transition name="fade">
            <mu-row gutter align-items="center" v-show="sayingChosen">
              <mu-col span="9" offset="3">
                <mu-button color="primary" v-on:click="addvotedInfo">添加</mu-button>
              </mu-col>
            </mu-row>
          </transition>
        </mu-form>
      </div>
      <transition name="fade">
        <div v-show="voted">
          <h3>已投群员</h3>
          <mu-row gutter align-items="center">
            <mu-col span="12">
              <mu-chip
                class="chip"
                v-for="(votedInfo, index) in votedInfos"
                :key="votedInfo.name"
                @delete="removeVotedInfo(index)"
                delete
                color="blue300"
              >{{votedInfo.name}}</mu-chip>
            </mu-col>
          </mu-row>
          <mu-row gutter align-items="center">
            <mu-button :color="this.submitButtonStatus.color" v-on:click="submitAll()">{{this.submitButtonStatus.text}}</mu-button>
          </mu-row>
        </div>
      </transition>
    </mu-flex>
  </main-outline>
</template>

<script lang="ts">
import MainOutline from '../components/MainOutline.vue'
import { AlignmentInfo, AlignmentDatabase } from '../components/AlignmentInfo.vue'
import Database from '../assets/database'
import Vue from 'vue'

export default Vue.extend({
  name: 'Vote',
  data () {
    return {
      votedInfos: [] as Array<object>,
      currentInfo: {
        name: '',
        alignment: '',
        saying: ''
      },
      database: Database,
      submitButtonStatus: {
        color: 'secondary',
        text: '提交全部'
      }
    }
  },
  computed: {
    usernames (): Array<string> {
      return Object.keys(this.database)
    },
    alignments (): Array<string> {
      return Object.keys(AlignmentDatabase)
    },
    sayings (): Array<string> {
      const name = this.currentInfo.name as keyof typeof Database
      if (name in Database) {
        return Database[name]
      }
      return []
    },
    voted (): boolean {
      return this.votedInfos.length > 0
    },
    usernameChosen (): boolean {
      return this.currentInfo.name !== ''
    },
    alignmentChosen (): boolean {
      return this.currentInfo.alignment !== ''
    },
    sayingChosen (): boolean {
      return this.currentInfo.saying !== ''
    }
  },
  methods: {
    removeVotedInfo (index: number): void {
      this.votedInfos.splice(index, 1)
    },
    addvotedInfo (): void {
      const voteInfo = {
        name: this.currentInfo.name,
        alignment: this.currentInfo.alignment,
        saying: this.currentInfo.saying
      }
      this.votedInfos.push(voteInfo)
      this.currentInfo.name = ''
      this.currentInfo.alignment = ''
      this.currentInfo.saying = ''
    },
    cleanSaying (): void {
      this.currentInfo.saying = ''
    },
    async submitAll (): Promise<void> {
      this.submitButtonStatus.text = '正在提交...'
      this.submitButtonStatus.color = 'secondary'
      try {
        const response = await fetch('http://localhost:5000/api/Vote', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify(this.votedInfos)
        })
        if (!response.ok) {
          // console.log(await response.json())
          this.submitButtonStatus.text = '发生错误，请联系群管理员'
          this.submitButtonStatus.color = 'error'
        } else {
          this.$router.push('thanks')
        }
      } catch (err) {
        // console.log(err)
        this.submitButtonStatus.text = '发生错误，请联系群管理员'
        this.submitButtonStatus.color = 'error'
      }
    }
  },
  components: {
    MainOutline,
    AlignmentInfo
  }
})
</script>

<style lang="scss">
.vote-form {
  width: 100%;
}
.row {
  transition: all 0.45s cubic-bezier(0.23, 1, 0.32, 1);
}
.fade-enter-active,
.fade-leave-active {
  transition: all 0.45s cubic-bezier(0.23, 1, 0.32, 1);
}
.fade-enter,
.fade-leave-to {
  opacity: 0;
}
.chip {
  margin: {
    right: 10px;
    bottom: 20px;
  }
}
</style>
