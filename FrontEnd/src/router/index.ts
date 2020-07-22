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
  },
  {
    path: '/vote',
    name: 'Vote',
    component: () => import(/* webpackChunkName: "Vote" */ '../views/Vote.vue')
  },
  {
    path: '/nominate',
    name: 'Nominate',
    component: () => import(/* webpackChunkName: "Nominate" */ '../views/Nominate.vue')
  }
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

export default router
