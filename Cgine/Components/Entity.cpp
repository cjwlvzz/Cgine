#include "Entity.h"

namespace Cgine::game_entity
{
	namespace
	{
		util::vector<Cgine::id::generation_type> generations;
	}

	entity_id Cgine::game_entity::create_game_entity(const entity_info& info)
	{
		assert(info.transform);
		if (!info.transform)
		{
			return entity_id();
		}
	}

	void Cgine::game_entity::remove_game_entity(entity_id id)
	{
	}

	bool Cgine::game_entity::is_alive(entity_id id)
	{
		return false;
	}
}