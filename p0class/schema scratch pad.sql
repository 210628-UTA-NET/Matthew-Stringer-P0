CREATE TABLE products(
    p_id int IDENTITY,
    p_name varchar(20) NOT NULL,
    p_price DECIMAL(15,2) NOT NULL,
    p_desc varchar(120),
    p_category varchar(20)
)

alter table products add CONSTRAINT pk_products PRIMARY KEY (p_id)

create table line_items(
    l_id  int IDENTITY,
    l_prod int NOT NULL,
    l_order int,
    l_storefront int,
    l_quantity int NOT NULL,
    CONSTRAINT pk_line_items PRIMARY KEY(l_id),
    CONSTRAINT fk_line_items_products FOREIGN KEY(l_prod) REFERENCES dbo.products (p_id),
    CONSTRAINT fk_line_items_orders FOREIGN KEY(l_order) REFERENCES dbo.orders (o_id),
    CONSTRAINT fk_line_items_store_front FOREIGN KEY(l_storefront) REFERENCES store_front (s_id),
    CONSTRAINT item_inventory_or_order CHECK ((l_order is null or l_storefront is null) and not (l_order is null and l_storefront is null))
)

drop table line_items

CREATE TABLE orders(
    o_id int IDENTITY,
    o_loc VARCHAR(30) NOT NULL,
    o_price DECIMAL(20,2) NOT NULL,
    o_store int NOT NULL,
    CONSTRAINT pk_orders PRIMARY KEY(o_id),
    CONSTRAINT fk_orders_store_front FOREIGN KEY(o_store) REFERENCES store_front (s_id)
)
DROP TABLE orders

create table customer(
    c_id int IDENTITY,
    c_name varchar(30),
    c_addr varchar(90),
    c_email varchar(30),
)

create table store_front(
    s_id int IDENTITY,
    c_addr VARCHAR(90),
    CONSTRAINT pk_store_front PRIMARY KEY (s_id)
)
alter table store_front add s_addr VARCHAR(90);