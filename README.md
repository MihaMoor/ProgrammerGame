##Прототипы
https://www.old-games.ru/forum/threads/simuljatory-kompjuterschika-dlja-opisi-na-sajte-do-2005-goda-vypuska.91839/
https://www.old-games.ru/game/9770.html

https://herbertograca.com/2017/11/16/explicit-architecture-01-ddd-hexagonal-onion-clean-cqrs-how-i-put-it-all-together/
https://docs.google.com/drawings/d/1E_hx5B4czRVFVhGJbrbPDlb_JFxJC8fYB86OMzZuAhg/edit?pli=1
 ![Infographic 16_9](https://github.com/user-attachments/assets/e5b46f5b-7183-49f2-8d1d-b80e029ebe95)


 Для просмотра логов в Kibana необходимо создать DataView, в котором указать паттерн из logstash.conf, раздел index. 
 На данный момент: server-logs-*

 Для просмотра метрик необходимо:
- зайти в Grafana
- перейти на вкладку Data Source
- выбрать Prometheus
- в поле Connection вбить http://prometheus:9090
- проскролить в самый низ и нажать на кнопку Save & test
Если что-то пошло не так - гуглить))