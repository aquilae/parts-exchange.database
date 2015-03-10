create type [dbo].[fn_binary_sort_input] as table
(
    id bigint not null,
    value varbinary(8000) not null
);
