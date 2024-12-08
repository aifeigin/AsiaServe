docker build -f dockerfile.build -t asiaserve .
docker create --name asiaserv-box asiaserve
docker cp asiaserv-box:/app ../PublishNix
