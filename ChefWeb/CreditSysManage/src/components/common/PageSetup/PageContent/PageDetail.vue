<template>
	<div class="pageDetail">
		<Modal title="对话框标题" v-model="modal" width='400' :mask-closable="false" class="modal">
			<ul>
	        		<li>
	        			<span>描述</span>
	        			<Input v-model="describe" placeholder="请输入..." style="width: 200px;margin-left: 3rem;"></Input>
	        		</li>
	        		<li>
	        			<span>跳转链接</span>
	        			<Input v-model='inputtext' type="textarea" :autosize="{minRows: 1,maxRows: 5}" placeholder="请输入..." style="width: 200px;margin-left: 1.5rem;"></Input>
	        		</li>
	        		<li>
	        			<div style="overflow: auto;">
	        				<span style="display: block;float: left;height: 5rem;line-height: 5rem;">图片</span>
	        				<img  src="../../../../../static/image/placeholderBag.png" style="max-height: 5rem;display: block;float: left;margin-left: 3.2rem;"/>
	        			</div>
	        		</li>
	        </ul>
	    </Modal>
		<Table style="margin-top: 1rem;" :columns="pageDetail_columns" :data="pageDetail_tableList"></Table>
	</div>
</template>
<script>
	export default{
		data(){
			return{
				modal:false,//弹框
				describe:'',//弹框描述
				inputtext:'',//弹框链接
				pageDetail_tableList:[
				],
				pageDetail_columns:[
         			{	
         				title:'选择',
                        type: 'selection',
                        align: 'center',
                        width:80
                     },
         			{	
                        title:'NO.',
                        align: 'center',
                        key:'number',
                        width:70
                     },
         			{
                        title: '描述',
                        key: 'describe',
                        align: 'center',
                    },
                    {
                        title: '图片预览',
                        align: 'center',
                        key: 'Preview_picture',
                        render:(h,paeams) =>{
                        		return h('img',{
                        			attrs:{
                        				src:paeams.row.Preview_picture
                        			},
                        			style:{
                        				maxWidth:'5rem'
                        			}
                        		});
                        }
                    },
                     {
                        title: '跟新时间',
                        align: 'center',
                        key: 'Update_time',
                    },
                     {
                        title: '跳转链接',
                        align: 'center',
                        key: 'chaining'
                    },
                    {
                        title: '显示',
                        align: 'center',
                        key: 'display',
                        width:70
                    },
                    {
                        title: '排序',
                        align: 'center',
                        key: 'sorting',
                        width:80
                    },
                     {
                        title: '操作',
                        align: 'center',
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
                                }, '编辑')
                            ]);
                        }
                    },
         		],
			}
		},
		methods:{
			
		},
		mounted:function(){
			
		}
	}
</script>

<style scoped>
.modal ul li {
	display: block;
	margin: 0.5rem auto;
	
}
</style>