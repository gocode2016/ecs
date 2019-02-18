<template>
	<div class="pageList">
		<Table style="margin-top: 1rem;" :columns="page_columns" :data="page_tableList"></Table>
	</div>
</template>

<script>
	export default{
		data(){
			return{
				page_columns:[
         			{	
         				title:'No.',
//                      type: 'selection',
                        align: 'center',
                         key:'number'
                     },
         			{	
                        title:'页面标题',
                        align: 'center',
                        key:'SalemanId'
                        
                     },
         			{
                        title: '跟新时间',
                        key: 'RegionName',
                        align: 'center',
                     },
                     {
                        title: '操作',
                        align: 'center',
                        key: 'operation',
                        render: (h, params) => {
                            return h('div', [
                                h('Button', {
                                    props: {
                                        type: 'error',
                                        size: 'small'
                                    },
                                    style:{
                                    	width:'50px'
                                    },
                                    on: {
                                        click: () => {
                                            this.redact(params)
                                        }
                                    }
                                }, '查看')
                            ]);
                        }
                    },
         		],
         		page_tableList:[
         			{
         				number:'1',
         				SalemanId:'标题',
         				RegionName:'2017-8-28-08:30'
         			}
         		],
			}
		},
		methods:{
			redact(name){//编辑
				console.log(name.row)//PageDetail.vue
				//this.$router.push({path: '/PageDetail', query: {SalemanId: name.row.SalemanId,RegistState:name.row.RegistState}});//跳转页面内容
				//this.$router.push({path: '/PageDetail'});
			}
		},
		mounted:function(){
			
		}
	}
</script>

<style scoped>
.pageList{
	
}
</style>