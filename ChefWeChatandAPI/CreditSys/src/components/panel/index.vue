<template>
  <div class="weui-panel weui-panel_access">
    <div class="weui-panel__hd" v-if="header" @click="onClickHeader" v-html="header"></div>
    <div class="weui-panel__bd">
      <!--type==='1'-->
      <template v-if="type === '1'">
        <a :href="getUrl(item.url)" v-for="item in list"  @click.prevent="onItemStarClick(item.ChefStarId)" class="weui-media-box weui-media-box_appmsg" :id="item.class">
          <div class="weui-media-box__hd" v-if="item.src">
            <img class="weui-media-box__thumb" :src="item.src" alt="">
          </div>
          <div class="weui-media-box__bd">
            <h4 class="weui-media-box__title">{{item.title}}</h4>
            <p class="weui-media-box__desc">{{item.desc}}</p>
          </div>
        </a>
      </template>
      <!--type==='2'-->
      <template v-if="type === '2'">
        <div class="weui-media-box weui-media-box_text" v-for="item in list" @click.prevent="onItemClick(item)">
            <h4 class="weui-media-box__title">{{item.title}}</h4>
            <p class="weui-media-box__desc">{{item.desc}}</p>
        </div>
      </template>
      <!--type==='3'-->
      <template v-if="type === '3'">
        <div class="weui-media-box weui-media-box_small-appmsg">
            <div class="weui-cells">
              <a class="weui-cell weui-cell_access" :href="getUrl(item.url)" v-for="item in list" @click.prevent="onItemClick(item)">
                <div class="weui-cell__hd">
                  <img :src="item.src" alt="" style="width:20px;margin-right:5px;display:block">
                </div>
                <div class="weui-cell__bd">
                  <p>{{item.title}}</p>
                </div>
                <span class="weui-cell__ft"></span>
              </a>
            </div>
        </div>
      </template>
      <!--type==='4'-->
      <template v-if="type === '4'">
        <div class="weui-media-box weui-media-box_text" v-for="item in list" @click.prevent="onItemClick(item)">
          <h4 class="weui-media-box__title">{{item.title}}</h4>
          <p class="weui-media-box__desc">{{item.desc}}</p>
          <ul class="weui-media-box__info" v-if="item.meta">
            <li class="weui-media-box__info__meta">{{item.meta.source}}</li>
            <li class="weui-media-box__info__meta">{{item.meta.date}}</li>
            <li class="weui-media-box__info__meta weui-media-box__info__meta_extra">{{item.meta.other}}</li>
          </ul>
        </div>
      </template>
      <!--type==='5'-->
      <template v-if="type === '5'">
        <div class="weui-media-box weui-media-box_text" v-for="item in list" @click.prevent="onItemClick(item)">
          <div class="weui-media-box_appmsg">
            <div class="weui-media-box__hd" v-if="item.src">
              <img class="weui-media-box__thumb" :src="item.src" alt="">
            </div>
            <div class="weui-media-box__bd">
              <h4 class="weui-media-box__title">{{item.title}}</h4>
              <p class="weui-media-box__desc">{{item.desc}}</p>
            </div>
          </div>
          <ul class="weui-media-box__info" v-if="item.meta">
            <li class="weui-media-box__info__meta">{{item.meta.source}}</li>
            <li class="weui-media-box__info__meta">{{item.meta.date}}</li>
            <li class="weui-media-box__info__meta weui-media-box__info__meta_extra">{{item.meta.other}}</li>
          </ul>
        </div>
      </template>
      <!--type==='6'-->
      <template v-if="type === '6'">
        <a :href="getUrl(item.url)" v-for="item in list" @click.prevent="onItemClick(item.tutorId)" class="weui-media-box weui-media-box_appmsg" :id="item.class">
          <div class="weui-media-box__hd" v-if="item.src">
            <img class="weui-media-box__thumb" :src="item.src" alt="">
          </div>
          <div class="weui-media-box__bd">
            <h4 class="weui-media-box__title">{{item.title}}</h4>
            <h2 class="weui-media-box__title small_title">{{item.smallTitle}}</h2>
            <p class="weui-media-box__desc weui-media-box__des">{{item.desc}}</p>
            <p class="weui-media-box__desc">{{item.category}}</p>
          </div>
        </a>
      </template>

    </div>
    <div class="weui-panel__ft">
      <a class="weui-cell weui-cell_access weui-cell_link" :href="getUrl(footer.url)" v-if="footer && type !== '3'" @click.prevent="onClickFooter">
        <div class="weui-cell__bd" v-html="footer.title"></div>
      </a>
    </div>
  </div>
</template>

<script>
import { go, getUrl } from '../../libs/router'

export default {
  name: 'panel',
  props: {
    header: String,
    footer: Object,
    list: Array,
    type: {
      type: String,
      default: '1'
    }
  },
  methods: {
    getUrl (url) {
      return getUrl(url, this.$router)
    },
    onClickFooter () {
      this.$emit('on-click-footer')
      go(this.footer.url, this.$router)
    },
    onClickHeader () {
      this.$emit('on-click-header')
    },
    onItemStarClick(ChefStarId){
//  	go(item.url, this.$router),query:{tutorId:tutorId}
      this.$router.push({path:'/component/starkitchen',query:{chefStarId:ChefStarId}})
    },
    onItemClick (tutorId) {
//    this.$emit('on-click-item', item)
//    go(item.url, this.$router)
      this.$router.push({path:'/component/tutor',query:{tutorId:tutorId}})
    }
  }
}
</script>

<style lang="less">
@import '../../styles/weui/widget/weui_cell/weui_cell_global';
@import '../../styles/weui/widget/weui_cell/weui_access';
@import '../../styles/weui/widget/weui_panel/weui_panel';
@import '../../styles/weui/widget/weui_media_box/weui_media_box';

.weui-panel .weui-cell:first-child:before {
  display: block;
}

.weui-media-box__title.small_title { font-size: 14px; }
#odd{background:#fbfbfb;}
</style>
