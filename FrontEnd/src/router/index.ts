import Vue from 'vue'
import VueRouter from 'vue-router'
import Vote from '../views/Vote.vue'

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Vote
  },
  {
    path: '/thanks',
    name: 'Thanks',
    component: () => import(/* webpackChunkName: "thanks" */ '../views/Thanks.vue')
  },
  {
    path: '/voteResult',
    name: 'VoteResult',
    component: () => import(/* webpackChunkName: "voteResult" */ '../views/VoteResult.vue')
  }
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

export default router
