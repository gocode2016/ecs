<template>
  <div>
    <group title="switch the type">
      <radio title="type" v-model="type" :options="['1', '2', '3', '4', '5', '6']"></radio>
    </group>
    <panel header="图文组合列表" :footer="footer" :list="list" :type="type"></panel>
  </div>
</template>

<script>
import { Panel, Group, Radio } from 'vux'

export default {
  components: {
    Panel,
    Group,
    Radio
  },
  data () {
    return {
      type: '1',
      list: [{
        src: 'http://placeholder.qiniudn.com/60x60/3cc51f/ffffff',
        title: '标题一',
        smallTitle: '小标题',
        desc: '由各种物质组成的巨型球状天体，叫做星球。星球有一定的形状，有自己的运行轨道。',
        category: '赛区：山东 广东 河南 河北',
        url: '/component/cell'
      }, {
        src: 'http://placeholder.qiniudn.com/60x60/3cc51f/ffffff',
        title: '标题二',
        smallTitle: '小标题',
        desc: '由各种物质组成的巨型球状天体，叫做星球。星球有一定的形状，有自己的运行轨道。',
        category: '赛区：山东 广东 河南 河北',
        url: {
          path: '/component/radio',
          replace: false
        },
        meta: {
          source: '来源信息',
          date: '时间',
          other: '其他信息'
        }
      }],
      footer: {
        title: '查看更多',
        url: 'http://vux.li'
      }
    }
  }
}
</script>
