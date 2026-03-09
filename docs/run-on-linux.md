>[На главную](../README.md)
#

# Run in Linux

- Обновить систему: `sudo apt update && sudo apt upgrade -y`
- Установите .NET Runtime (или SDK, если нужно):  
    - `sudo apt update`  
    - `sudo apt install -y dotnet-runtime-10.0`
- Загрузить последний релиз на сервер и распоковать в папку */var/www/CottageSensoreAggregator*: [Релизы](https://github.com/MihaMoor/CottageSensorAggregator/releases)
- Создать пользователя для приложения: `sudo useradd -r -s /bin/false csauser`
- Назначить пользователя владельцем папки: `sudo chown -R myappuser:myappuser /var/www/CottageSensoreAggregator`
- Создать файл сервиса: `sudo nano /etc/systemd/system/cottage.service`  
Вставить следующее содержимое:
    ```
    [Unit]
    Description=Cottage sensore aggregator
    After=network.target

    [Service]
    Type=simple
    WorkingDirectory=/var/www/CottageSensoreAggregator
    User=csauser
    Group=csauser

    ExecStart=/usr/bin/dotnet /var/www/CottageSensoreAggregator/Api.dll

    Restart=always
    RestartSec=10

    # Логирование
    StandardOutput=journal
    StandardError=journal

    [Install]
    WantedBy=multi-user.target
    ```
- Исправить конфигурацию *appsettings.json* согласно [документации](appsettings.md)
- Перезагрузить daemon: `sudo systemctl daemon-reload`
- Включить автозапуск приложения: `sudo sustemctl enable cottage`
- Запустить сервис: `sudo systemctl start cottage`
- Проверить, что сервис запущен и добавлен в автозапуск: `systemctl status cottage`
- Перейти по *http* на *URL*, который был указан в *appsettings.json* в разделе *Kestrel*. Должен открыть Swagger.

## Обновление сервиса

- Скопировать файлы в папку */var/www/CottageSensoreAggregator* с заменой
- Проверить настройки в *appsettings.json*
- Перезагрузить сервис: `sudo systemctl restart cottage`
- Проверить, что сервис работает: `systemctl status cottage`

#
>[В начало](#)  
>[На главную](../README.md)
