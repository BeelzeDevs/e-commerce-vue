PGPASSWORD=hYAUhaUJuK69vQwYigZfr0o1PTVeQwHU \
psql \
-h dpg-d5a3jbje5dus73esq640-a.oregon-postgres.render.com \
-U ecommerce_vue_user \
-d ecommerce_vue \
-f database/docker-entry-initdb.d/init.sql

