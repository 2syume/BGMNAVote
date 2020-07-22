<template>
  <main-outline>
    <h3>投票结果</h3>
    <transition name="fade">
      <mu-alert color="error" v-if="dataLoadingError">
        <mu-icon left value="warning"></mu-icon>无法加载数据，请联系群管理员
      </mu-alert>
    </transition>
    <mu-data-table
      :columns="columns"
      :sort.sync="sort"
      @sort-change="handleSortChange"
      :data="tableData.data"
      :loading="dataLoading"
    >
      <template slot-scope="scope" class="fade">
        <td class="is-right fade">{{scope.row.nominateId}}</td>
        <td class="fade">{{scope.row.name}}</td>
        <td class="fade">{{scope.row.alignment}}</td>
        <td class="fade">{{scope.row.saying}}</td>
      </template>
    </mu-data-table>
    <mu-flex justify-content="end">
      <mu-pagination raised :page-size="1" :total="tableData.maxPage" :current.sync="tableData.page" @change="handlePageChange()"></mu-pagination>
    </mu-flex>
  </main-outline>
</template>

<script lang="ts">
import Vue from 'vue'
import MainOutline from '../components/MainOutline.vue'

interface Vote {
  voteId: number;
  name: string;
  saying: string;
  alignment: string;
}

interface VoteData {
  maxPage: number;
  page: number;
  data: Array<Vote>;
}

interface Sort {
  name: string;
  order: string;
}

export default Vue.extend({
  name: 'VoteResult',
  components: { MainOutline },
  data () {
    return {
      tableData: {
        maxPage: 1,
        page: 1,
        data: []
      } as VoteData,
      sort: {
        name: 'nominateId',
        order: 'desc'
      } as Sort,
      columns: [
        { title: 'Id', name: 'nominateId', sortable: true, width: 100 },
        { title: '被提名群员', name: 'name', sortable: true },
        { title: '阵营', name: 'alignment', sortable: true },
        { title: '台词', name: 'saying', sortable: false }
      ],
      dataLoading: false,
      dataLoadingError: false
    }
  },
  methods: {
    async fetchData (page?: number, orderBy = 'NominateId', asc = false) {
      this.dataLoading = true
      if (page === undefined) {
        try {
          page = parseInt(this.$route.query.page as string)
          if (page < 1) page = 1
        } catch {
          page = 1
        }
      }
      try {
        const response = await fetch(`https://bgmnavote.koromo.moe/api/Nominate?page=${page}&asc=${asc}&orderBy=${orderBy}`)
        if (!response.ok) {
          throw Error('cannot load data')
        }
        const data = await response.json() as VoteData
        this.tableData = data
        this.dataLoading = false
        this.dataLoadingError = false
      } catch (e) {
        // console.log(e)
        this.dataLoadingError = true
      }
    },
    handleSortChange ({ name, order }: Sort) {
      this.fetchData(1, name, order === 'asc')
    },
    handlePageChange () {
      this.fetchData(this.tableData.page, this.sort.name, this.sort.order === 'asc')
    }
  },
  created () {
    this.fetchData(undefined)
  }
})
</script>

<style>
.fade
.fade-enter-active,
.fade-leave-active {
  transition: all 0.45s cubic-bezier(0.23, 1, 0.32, 1);
}
.fade-enter,
.fade-leave-to {
  opacity: 0;
}
.mu-pagination {
  margin-top: 20px;
}
</style>
