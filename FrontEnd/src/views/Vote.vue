<template>
  <div>
    <transition name="fade">
      <vote-modal
        :voteInfo.sync="currentVoteInfo"
        v-if="currentVoteInfo.alignment!==''"
        @close="closeModal()"
      ></vote-modal>
    </transition>
    <mu-container class="container-top">
      <div class="intro">
        <div class="major-text">BGMNA 第三届群员九宫格活动</div>
        <div class="minor-text">群成员阵营印象划分——或者说九宫格——是群里的传统活动。主要目的是增进友谊和加深群成员之间的刻板印象。</div>
        <div class="minor-text">本次投票为正式投票阶段。</div>
        <br />
        <div class="minor-text">
          <a href="https://bgmna.org/2019/08/13/alignment-round-one.html" target="_blank">第一次九宫格活动结果</a>
        </div>
        <div class="minor-text">
          <a href="https://bgmna.org/2019/08/15/alignment-round-two.html" target="_blank">第二次九宫格活动结果</a>
        </div>
      </div>
      <mu-row gutter>
        <mu-col lg="4" md="6" span="12" v-for="votedInfo in votedInfos" :key="votedInfo.alignment">
          <alignment-slot-vote
            class="alignment-slot"
            :voteInfo="votedInfo"
            @click.native="showModal(votedInfo)"
          ></alignment-slot-vote>
        </mu-col>
      </mu-row>
      <mu-row justify-content="center">
        <mu-col lg="4" md="6" span="12">
          <transition name="fade">
            <div :class="submitButtonClass">
              <span class="button-text" @click="submitAll()">{{ submitButtonStatus.text }}</span>
            </div>
          </transition>
        </mu-col>
      </mu-row>
    </mu-container>
    <div class="footer">
      Made by&nbsp;
      <a href="https://github.com/arition" target="_blank">@arition</a>&nbsp; and &nbsp;
      <a href="https://github.com/ReventonC" target="_blank">@ReventonC</a>&nbsp; with ❤️️.
    </div>
  </div>
</template>

<script lang="ts">
import AlignmentSlotVote from '../components/AlignmentSlotVote.vue'
import VoteModal from '../components/VoteModal.vue'
import AlignmentDatabase from '../assets/AlignmentDatabase'
import Vue from 'vue'

interface Vote {
  userId: number;
  saying: string;
  alignment: string;
  nominateId: number;
}

export default Vue.extend({
  name: 'Vote',
  data () {
    return {
      submitButtonStatus: {
        text: '提交投票',
        color: 'secondary'
      },
      currentVoteInfo: { userId: -1, alignment: '', saying: '', nominateId: -1 },
      votedInfos: [] as Array<Vote>
    }
  },
  computed: {
    canSubmit (): boolean {
      return this.votedInfos.filter(t => t.userId !== -1).length > 0
    },
    submitButtonClass (): object {
      return {
        button: true,
        'button-inactive': this.votedInfos.filter(t => t.userId !== -1).length === 0,
        'button-secondary': this.votedInfos.filter(t => t.userId !== -1).length > 0 && this.votedInfos.filter(t => t.userId !== -1).length < 9,
        'button-primary': this.votedInfos.filter(t => t.userId !== -1).length === 9
      }
    }
  },
  methods: {
    async submitAll (): Promise<void> {
      this.submitButtonStatus.text = '正在提交...'
      this.submitButtonStatus.color = 'secondary'
      const submitInfo = this.votedInfos
        .filter(t => t.userId !== -1)
        .map(t => {
          return {
            nominateId: t.nominateId
          }
        })
      try {
        const response = await fetch('https://bgmnavote.koromo.moe/api/Vote', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify(submitInfo)
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
    },
    showModal (votedInfo: Vote) {
      this.currentVoteInfo.userId = votedInfo.userId
      this.currentVoteInfo.saying = votedInfo.saying
      this.currentVoteInfo.alignment = votedInfo.alignment
      this.currentVoteInfo.nominateId = votedInfo.nominateId
    },
    closeModal () {
      const votedInfo = this.votedInfos.filter(t => t.alignment === this.currentVoteInfo.alignment)[0]
      votedInfo.userId = this.currentVoteInfo.userId
      votedInfo.saying = this.currentVoteInfo.saying
      votedInfo.nominateId = this.currentVoteInfo.nominateId

      this.currentVoteInfo.alignment = ''
      this.currentVoteInfo.userId = -1
      this.currentVoteInfo.saying = ''
      this.currentVoteInfo.nominateId = -1
    }
  },
  created () {
    this.votedInfos = Object.keys(AlignmentDatabase).map((val) => {
      return {
        userId: -1,
        saying: '',
        alignment: val,
        nominateId: -1
      }
    })
  },
  components: {
    // AlignmentInfo,
    VoteModal,
    AlignmentSlotVote
  }
})
</script>

<style lang="scss" scoped>
.container-top {
  margin: {
    top: 40px;
    bottom: 40px;
  }
}
.intro {
  margin: {
    left: 10px;
    right: 10px;
    bottom: 40px;
  }
}
.alignment-slot {
  padding: 0px;
}
.major-text {
  font-style: normal;
  font-weight: bold;
  font-size: 24px;
  line-height: 30px;
  color: #ff4081;
  margin-bottom: 20px;
}
.minor-text {
  font-style: normal;
  font-weight: normal;
  font-size: 16px;
  line-height: 24px;
  color: rgba(60, 60, 60, 0.8);
}

.minor-text a {
  color: rgba($color: #333, $alpha: 0.4);
  transition: 0.2s;

  &:hover {
    transition: 0.2s;
    color: #ff4081;
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

.button {
  cursor: pointer;
  display: flex;
  border-radius: 10px;
  padding: 10px;
  margin: {
    bottom: 20px;
    top: 20px;
    left: 10px;
    right: 10px;
  }

  .button-text {
    font-weight: bold;
    margin: auto;
    width: 100%;
    text-align: center;
  }

  &-inactive {
    pointer-events: none;
    border: #e4e4e4 1px solid;
    background-color: #e4e4e4;
    .button-text {
      color: #aaaaaa;
    }
  }

  &-primary {
    border: #ff4081 1px solid;
    background-color: #ff4081;
    .button-text {
      color: white;
    }
    transition: 0.2s;
    &:hover {
      transition: 0.2s;
      opacity: 0.8;
    }
  }

  &-secondary {
    border: #ff4081 1px solid;
    background-color: transparent;
    .button-text {
      color: #ff4081;
    }
    transition: 0.2s;

    &:hover {
      transition: 0.2s;
      opacity: 0.8;
    }
  }
}

.footer {
  opacity: 0.7;
  font-size: 12px;
  bottom: 0;
  width: 100%;
  text-align: right;
  width: 100%;
  padding: 10px 20px;
  @media (min-width: 992px) {
    position: fixed;
  }

  transition: 0.2s;
  &:hover {
    opacity: 1;
    transition: 0.2s;
  }
}
</style>
