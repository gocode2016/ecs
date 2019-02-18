<template>
	<div>
		<Tree :data="data1" ref='tree'></Tree>
	</div>
	
</template>

<script>
	export default{
		data(){
			return{
				data1: [
                    {
                        title: '欣和企业',
                        expand: true,
                        children: [
                            {
                                title: '欣和工厂',
                                expand: true,
                                children: [
                                    {
                                        title: '车间1'
                                    },
                                    {
                                        title: '车间2'
                                    }
                                ]
                            },
                            {
                                title: '味达美工厂',
                                expand: true,
                                children: [
                                    {
                                        title: '车间1'
                                    },
                                    {
                                        title: '车间2'
                                    }
                                ]
                            }
                        ]
                    }
                ]
			}
		},
		methods:{
			
		},
		mounted:function(){
			
		}
	}
</script>

<style scoped>
</style>