<template>
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
      await getConfigChecComputerState();
      await fetchData();
      autoUpdateDate();
   } catch (error) {
      console.log(error);
   }
})

/**
 * lấy config thời gian check computer state
 */
const getConfigChecComputerState = async () => {
   try {
      let rs = await configOptionService.getByOptionName("CHECK_COMPUTER_STATE");
      if (rs && rs.data) {
         checkTime.value = Number(rs.data.optionValue) || checkTime.value;
      }
   } catch (error) {
      console.log(error);
   }
}
const fetchData = async () => {
   try {
      let rs = await computerService.getComputerOnLineChart(checkTime.value);
      if (rs && rs.data) {
         data.value = bindData(rs.data);
      }
   } catch (error) {

   }
}
const bindData = (data) => {
   return {
      labels: [$t("ChartComputerOnline.Labels.ComputerOnline"), $t("ChartComputerOnline.Labels.ComputerTotal")],
      datasets: [{
         label: $t("ChartComputerOnline.Label"),
         data: data,
         backgroundColor: [
            'rgba(255, 99, 132, 0.2)',
            'rgba(255, 159, 64, 0.2)',
         ],
         borderColor: [
            'rgb(255, 99, 132)',
            'rgb(255, 159, 64)',
         ],
         borderWidth: 1
      }]
   }
}
const autoUpdateDate = () => {
   if (interval.value) {
      clearInterval(interval.value);
   }
   interval.value = setInterval(async () => {
      await fetchData();
   }, checkTime.value)
}
</script>
<style lang="scss" scoped></style>