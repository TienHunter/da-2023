<template>
   <spin>

   </spin>
   <Bar :data="data" :options="options" />
</template>
<script setup>
import { computerService, configOptionService } from '@/api';
import {
   Chart as ChartJS,
   Title,
   Tooltip,
   Legend,
   BarElement,
   CategoryScale,
   LinearScale
} from 'chart.js'
import { onBeforeMount, ref } from 'vue';
import { Bar } from 'vue-chartjs'
ChartJS.register(Title, Tooltip, Legend, BarElement, CategoryScale, LinearScale)

const checkTime = ref(5000);
const interval = ref(null);
const data = ref({
   datasets: []
});
const options = {
   responsive: true,
   maintainAspectRatio: false
}
onBeforeMount(async () => {
   try {
      await fetchData();
   } catch (error) {
      console.log(error);
   }
})

const fetchData = async () => {
   try {
      let rs = await computerService.getComputerByListErrorChart();
      if (rs && rs.data) {
         data.value = bindData(rs.data);
      }
   } catch (error) {

   }
}
const bindData = (data) => {
   return {
      labels: [$t("ChartComputerCondition.Labels.Perfect"), $t("ChartComputerCondition.Labels.Hardware"), $t("ChartComputerCondition.Labels.Software"), $t("ChartComputerCondition.Labels.Network"), $t("ChartComputerCondition.Labels.OS"), $t("ChartComputerCondition.Labels.Total")],
      datasets: [{
         label: $t("ChartComputerCondition.Label"),
         data: data,
         backgroundColor: [
            'rgba(255, 99, 132, 0.2)',
            'rgba(255, 159, 64, 0.2)',
            'rgba(255, 205, 86, 0.2)',
            'rgba(75, 192, 192, 0.2)',
            'rgba(54, 162, 235, 0.2)',
            'rgba(153, 102, 255, 0.2)',
         ],
         borderColor: [
            'rgb(255, 99, 132)',
            'rgb(255, 159, 64)',
            'rgb(255, 205, 86)',
            'rgb(75, 192, 192)',
            'rgb(54, 162, 235)',
            'rgb(153, 102, 255)',
         ],
         borderWidth: 1
      }]
   }
}
</script>
<style lang="scss" scoped></style>